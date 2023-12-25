using demo.Models;
using Hospital_Management_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Hospital_Management_System.Controllers
{
	public class DoctorController : Controller
    {
        HMSEntites context = new HMSEntites();



        //*** Displaying Appointments Logic ***//
        public IActionResult DisplayAppointment(int _docID)
        {
            var query = from a in context.Appointments
                        join p in context.Patients
                        on a.Patient_id equals p.ID
                        where a.Status != "Canceled" && a.Doctor_id == _docID
                        orderby a.Date descending
                        select new PatientAppointmentVM
                        {
                            FullName = p.FullName,
                            Appointment = a
                        };

            var appointments = query.ToList();

            return View("DisplayAppointment", appointments);
        }


        //*** Creating Appointments Logic ***//
        public IActionResult CreateAppointment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAppointment(Appointment appointment) 
        {
            if (appointment.Date <= DateTime.Now)
            {
                ModelState.AddModelError("Date", "Appointment Date must be in the future");
                return View(appointment);
            }

            if (context.Patients.Any(p => p.ID == appointment.Patient_id))
            {
                context.Appointments.Add(appointment);
                context.SaveChanges();
                return RedirectToAction("DisplayAppointment", new { _docID = appointment.Doctor_id });
            }
            ModelState.AddModelError("Patient_id", "This Patient doesn't Exist");

            return View(appointment);

        }

        //*** Viewing Appointment details with the associated patient  ***//
        public IActionResult Details(int _patientid, int _docid, int _appointmentid)
        {
            
            var query = from p in context.Patients
                        join a in context.Appointments on p.ID equals a.Patient_id
                        join r in context.MedicalRecords on p.ID equals r.Patient_id
                        into detailedAppointment
                        where p.ID == _patientid && a.Doctor_id == _docid && a.Id == _appointmentid
                        select new PatientAppointmentMedicalRecordVM
                        {
                            Patient = p,
                            Appointment = a,
                            MedicalRecords = context.MedicalRecords.Where(mr => mr.Patient_id == _patientid).ToList()
                        };
            try 
            { 

                PatientAppointmentMedicalRecordVM appointmentDetails = query.Single();
                TempData["appointmentID"] = appointmentDetails.Appointment.Id;

                if (appointmentDetails != null)
                {
                    int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                    int bdate = int.Parse(appointmentDetails.Patient.BDate.ToString("yyyyMMdd"));
                    ViewData["age"] = (now - bdate) / 10000;
                    return View(appointmentDetails);
                }
                else
                { 
                    return RedirectToAction("DisplayAppointment", new { _docid }); 
                }
                
            }
            catch
            {
                return View();
            }
        }

        //*** Adding Medical Records Logic ***//
        public IActionResult AddMedicalRecord()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddMedicalRecord(MedicalRecord _medicalRecord)
        {

            if (context.Patients.Any(p => p.ID == _medicalRecord.Patient_id))
            {
                context.MedicalRecords.Add(_medicalRecord);
                context.SaveChanges();
                return RedirectToAction("Details", new { _docid = _medicalRecord.Doctor_id, _patientid = _medicalRecord.Patient_id, _appointmentid = TempData["appointmentID"] });
            }
            ModelState.AddModelError("Patient_id", "This Patient doesn't Exist");
            return View(_medicalRecord);
        }


        //*** updating appointment's status ***//

        [HttpPost]
        public IActionResult UpdateAppointmentStatus(Appointment appointment)
        {
            if (appointment != null)
            {
                context.Appointments.Update(appointment);
                context.SaveChanges();
                return Json(new { success = true, message = "Status Updated", newStatus = appointment.Status} );
            }
            else
            {
                return Json(new { success = false, message = "Failed to update status" });
            }
        }
    }
}
