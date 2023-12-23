using demo.Models;
using Hospital_Management_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Hospital_Management_System.Controllers
{
    public class PatientController : Controller
    {
        HMSEntites context = new HMSEntites();
        public IActionResult _Patient()
        {
            int patientId = 5; // Assuming patient ID is 5
            var patient = context.Patients.FirstOrDefault(i => i.ID == patientId);
            string name = patient.FullName;

            // Set the patient's name in ViewBag
            ViewBag.Name = name;

            return View("_Patient");
        }


        public IActionResult CreateAppointment()
        {
            // Get the list of doctors (staff with role "Doctor") for the dropdown
            var doctors = context.Staffs.Where(s => s.Role == "Doctor").ToList();
            ViewBag.Doctors = new SelectList(doctors, "ID", "FullName");

            return View();
        }

        [HttpGet]
        public IActionResult CreateAppointment(int Patient_id, int Doctor_id, DateTime Date, string Status)
        {
            // Check for appointment conflicts with the selected doctor
            bool hasConflict = context.Appointments.Any(a => a.Doctor_id == Doctor_id && a.Date == Date);

            if (hasConflict)
            {
                ModelState.AddModelError("Date", "Appointment time conflicts with an existing appointment for the selected doctor.");
            }
            Patient_id = 5;

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

        // Helper method to generate a new ID for the appointment
        private int GenerateNewAppointmentId()
        {
            // Logic to generate a new ID (you can use a database sequence, a GUID, etc.)
            // For simplicity, you can increment the highest existing ID by 1
            int maxId = context.Appointments.Max(a => (int?)a.Id) ?? 0;
            return maxId + 1;
        }


      


        public IActionResult DisplayAppointment(int patientid)
        {
            patientid = 5;

            List<Appointment> appointments = context.Appointments.Where(appointemnt=>appointemnt.Patient_id == patientid).ToList();
           
            return View("DisplayAppointment", appointments);
        }

        public IActionResult DisplayMedicalRecord(int id)
        {
            id = 5;
            List<MedicalRecord> MedRec = context.MedicalRecords.Where(mr => mr.Patient_id == id).ToList();

            return View("DisplayMedicalRecord", MedRec);
        }



        public IActionResult Details(int patientid)
        {
            patientid = 5;
            var patient = context.Patients.SingleOrDefault(i => i.ID == patientid);
             
            return View("Details", patient);
        }


		public IActionResult AppointementDetail(int _appointmetid, int patientid)
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
                    return RedirectToAction("AppointementDetail", new { _appointmetid = appointmentId, patientid = 5 });
                }

                ModelState.AddModelError("newDate", "Appointment time conflicts with an existing appointment for the selected doctor.");
            }

            // If there are validation errors or conflicts, redisplay the form
            var viewModel = new AppointmentUpdateViewModel
            {
                Appointment = appointment,
                CurrentDate = DateTime.Now
            };

            return View("UpdateAppointmentDate", viewModel);
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


        public List<PatientAppointmentMedicalRecordVM> GetDetail(int id)
        {
            id = 84;
            var query = from p in context.Patients
                        where id == p.ID
                        join m in context.MedicalRecords on id equals m.Patient_id
                        join a in context.Appointments on id equals a.Patient_id
                        join d in context.Doctors on a.Doctor_id equals d.Id
                        join s in context.Staffs on d.Id equals s.ID

                        select new PatientAppointmentMedicalRecordVM
                        {

                            Patient = p,
                            Appointment = a,
                          
                            Staff =s,
                            doctor = d
                        };

            return query.ToList();
        }
    }
}