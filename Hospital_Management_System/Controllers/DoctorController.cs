using demo.Models;
using Hospital_Management_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Security.Cryptography.X509Certificates;

namespace Hospital_Management_System.Controllers
{
    public class DoctorController : Controller
    {
        HMSEntites context = new HMSEntites();
        
        public IActionResult CreateAppointment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAppointment(Appointment appointment) 
        {
            context.Appointments.Add(appointment);
            context.SaveChanges();
            return RedirectToAction("DisplayAppointment");
        }
        public IActionResult DisplayAppointment()
        {
            

            var query = from a in context.Appointments
                        join p in context.Patients
                        on a.Patient_id equals p.ID
                        where a.Status != "Canceled"
                        orderby a.Date descending
                        select new PatientAppointmentViewModel 
                        {
                            FullName = p.FullName,
                           Appointment = a 
                        };



            var appointments = query.ToList();
            

            return View("DisplayAppointment", appointments);
        }

        public IActionResult AddMedicalRecord()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddMedicalRecord(MedicalRecord MRecord)
        {
            context.MedicalRecords.Add(MRecord);
            context.SaveChanges();
            return RedirectToAction("Details");
        }


        //public IActionResult DisplayMedicalRecords(int id)
        //{
        //    List<MedicalRecord> MedRec = context.MedicalRecords.Where(mr => mr.Patient_id == id).ToList();

        //    return View("ViewRecords", MedRec);
        //}

        public IActionResult Details(int _appointmetid, int _patientid)
        {
            var query = from p in context.Patients
                        join a in context.Appointments on p.ID equals a.Patient_id
                        join r in context.MedicalRecords on p.ID equals r.Patient_id
                        into detailedAppointment
                        select new PatientAppointmentMedicalRecordVM
                        {
                            Patient = p,
                            Appointment = a,
                            MedicalRecords = context.MedicalRecords.Where(mr => mr.Patient_id == _patientid).ToList()
                        };


            var appointmentDetails = query.ToList();
            return View(appointmentDetails);
        }
    }
}
