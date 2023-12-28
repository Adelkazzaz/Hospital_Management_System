using demo.Models;
using Hospital_Management_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Hospital_Management_System.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        HMSEntites context = new HMSEntites();


        public IActionResult CreateAppointment()
        {
            // Get the list of doctors (staff with role "Doctor") for the dropdown
            try
            {
                var doctors = context.Staffs.Where(s => s.Role == "Doctor").ToList();
                ViewBag.Doctors = new SelectList(doctors, "ID", "FullName");

                var patientIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "patientid");

                if (patientIdClaim != null && int.TryParse(patientIdClaim.Value, out int patientId))
                {
                    ViewBag.PatientId = patientId;
                }

                return View();
            }
            catch (Exception ex)
            {
                // Handle the exception (log, redirect, show error message, etc.)
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult CreateAppointment(int Doctor_id, DateTime Date, string Status)
        {
            try
            {
                // Check for appointment conflicts with the selected doctor
                bool hasConflict = context.Appointments.Any(a => a.Doctor_id == Doctor_id && a.Date == Date);

                if (Date < DateTime.Now)
                {
                    ModelState.AddModelError("Date", " note! [Appointment date cannot be in the past].");
                }

                if (hasConflict)
                {
                    ModelState.AddModelError("Date", "Appointment time conflicts with an existing appointment for the selected doctor.");
                }

                // Retrieve the patient id from claims
                var patientIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "patientid");

                if (!int.TryParse(patientIdClaim?.Value, out int Patient_id))
                {
                    ModelState.AddModelError("", "Unable to retrieve patient information.");
                }

                // Validate the selected doctor
                var selectedDoctor = context.Staffs.FirstOrDefault(s => s.Role == "Doctor" && s.ID == Doctor_id);
                if (selectedDoctor == null)
                {
                    ModelState.AddModelError("Doctor_id", "Invalid doctor selection.");
                }


                if (ModelState.IsValid && !hasConflict)
                {
                    // Set the status to "Scheduled"
                    Status = "Scheduled";

                    // Create a new appointment
                    var appointment = new Appointment
                    {
                        Patient_id = Patient_id,
                        Doctor_id = Doctor_id,
                        Date = Date,
                        Status = Status,
                        // Assuming you want to generate a new ID for the appointment
                        Id = GenerateNewAppointmentId()
                    };

                    // Save the appointment to the database
                    context.Appointments.Add(appointment);
                    context.SaveChanges();

                    // Redirect to the Details action after saving the appointment
                    return RedirectToAction("Details", new { patientid = appointment.Patient_id });
                }

                // If there are validation errors or conflicts, reload the list of doctors and redisplay the form
                ViewBag.Doctors = new SelectList(context.Staffs.Where(s => s.Role == "Doctor"), "ID", "FullName");
                return View();
            }
            catch (Exception ex)
            {
                // Handle the exception (log, redirect, show error message, etc.)
                return RedirectToAction("Error", "Home");
            }

        }


        // Helper method to generate a new ID for the appointment
        private int GenerateNewAppointmentId()
        {
            // Logic to generate a new ID (you can use a database sequence, a GUID, etc.)
            // For simplicity, you can increment the highest existing ID by 1
            int maxId = context.Appointments.Max(a => (int?)a.Id) ?? 0;
            return maxId + 1;
        }


        public IActionResult DisplayAppointment()
        {
            try
            {
                // Retrieve the patient id from claims
                var patientIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "patientid");

                if (patientIdClaim != null && int.TryParse(patientIdClaim.Value, out int patientId))
                {
                    // Fetch appointments for the logged-in patient
                    List<Appointment> appointments = context.Appointments
                        .Where(appointment => appointment.Patient_id == patientId)
                        .ToList();

                    return View("DisplayAppointment", appointments);
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the exception)
                return StatusCode(500, "An error occurred while processing the request.");
            }
            // If patient ID cannot be retrieved, handle accordingly (e.g., show an error message or redirect)
            return RedirectToAction("Error", "Home");
        }


        public IActionResult DisplayMedicalRecord()
        {
            try
            {
                // Retrieve the patient id from claims
                var patientIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "patientid");

                if (patientIdClaim != null && int.TryParse(patientIdClaim.Value, out int patientid))
                {
                    List<MedicalRecord> MedRec = context.MedicalRecords
                        .Where(mr => mr.Patient_id == patientid)
                        .ToList();

                    return View("DisplayMedicalRecord", MedRec);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the exception)
                return StatusCode(500, "An error occurred while processing the request.");
            }

            // If patient ID cannot be retrieved, handle accordingly (e.g., show an error message or redirect)
            return RedirectToAction("Error", "Home");
        }



        public IActionResult Details(int patientid)
        {
            try
            {
                var patient = context.Patients.SingleOrDefault(i => i.ID == patientid);

                return View("Details", patient);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the exception)
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        public IActionResult AppointementDetail(int _appointmetid)
        {
            try
            {
                // Retrieve the patient id from claims
                var patientIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "patientid");

                if (patientIdClaim != null && int.TryParse(patientIdClaim.Value, out int patientid))
                {
                    var appointmentDetails = (from p in context.Patients
                                              join a in context.Appointments on p.ID equals a.Patient_id
                                              join d in context.Doctors on a.Doctor_id equals d.Id
                                              join s in context.Staffs on d.Id equals s.ID
                                              where a.Id == _appointmetid && p.ID == patientid
                                              select new PatientAppointmentViewModel
                                              {
                                                  patient = p,
                                                  Doctors = d,
                                                  staff = s,
                                                  Appointment = a
                                              }).FirstOrDefault();

                    return View("AppointementDetail", appointmentDetails);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the exception)
                return StatusCode(500, "An error occurred while processing the request.");
            }

            return RedirectToAction("Error", "Home");
        }



        public IActionResult UpdateAppointmentDate(int appointmentId)
        {


            var viewModel = new AppointmentUpdateViewModel
            {
                // Populate the view model with appointment details
                Appointment = context.Appointments.FirstOrDefault(a => a.Id == appointmentId),
                CurrentDate = DateTime.Now
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateAppointmentDate(int appointmentId, DateTime newDate)
        {
            try
            {
                var appointment = context.Appointments.FirstOrDefault(a => a.Id == appointmentId);

                if (appointment != null)
                {
                    // Check for conflicts with the selected doctor
                    bool hasConflict = context.Appointments.Any(a => a.Doctor_id == appointment.Doctor_id && a.Date == newDate && a.Id != appointmentId);

                    if (!hasConflict)
                    {
                        // Update the appointment date
                        appointment.Date = newDate;
                        context.SaveChanges();

                        // Redirect to the AppointementDetail action after updating the date
                        return RedirectToAction("AppointementDetail", new { _appointmetid = appointmentId, patientid = appointment.Patient_id });
                    }
                    if (newDate < DateTime.Now)
                    {
                        ModelState.AddModelError("Date", " note! [Appointment date cannot be in the past].");
                    }

                    ModelState.AddModelError("newDate", "Appointment time conflicts with an existing appointment for the selected doctor.");
                    var viewModel = new AppointmentUpdateViewModel
                    {
                        Appointment = appointment,
                        CurrentDate = DateTime.Now
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the exception)
                return StatusCode(500, "An error occurred while processing the request.");
            }
            return View();

        }

        [HttpPost]
        public IActionResult DeleteAppointment(int appointmentId)
        {
            var appointment = context.Appointments.FirstOrDefault(a => a.Id == appointmentId);

            if (appointment != null)
            {
                // Set the appointment status to "Canceled"
                appointment.Status = "Canceled";
                context.SaveChanges();
            }

            // Redirect to the AppointementDetail action after deleting the appointment
            return RedirectToAction("AppointementDetail", new { _appointmetid = appointmentId, patientid = 5 });
        }


        //public List<PatientAppointmentMedicalRecordVM> GetDetail(int id)
        //{
        //    id = 84;
        //    var query = from p in context.Patients
        //                where id == p.ID
        //                join m in context.MedicalRecords on id equals m.Patient_id
        //                join a in context.Appointments on id equals a.Patient_id
        //                join d in context.Doctors on a.Doctor_id equals d.Id
        //                join s in context.Staffs on d.Id equals s.ID

        //                select new PatientAppointmentMedicalRecordVM
        //                {

        //                    Patient = p,
        //                    Appointment = a,

        //                    Staff =s,
        //                    doctor = d
        //                };

        //    return query.ToList();
        //}
    }
}