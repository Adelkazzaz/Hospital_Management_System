using demo.Models;
using Hospital_Management_System.Models;
using Hospital_Management_System.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInMAnager;
        public HMSEntites context = new HMSEntites();

        public AccountController
            (UserManager<ApplicationUser> _UserManager,
            SignInManager<ApplicationUser> _SignInMAnager)
        {
            userManager = _UserManager;
            signInMAnager = _SignInMAnager;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel UserVm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userModel = await userManager.FindByNameAsync(UserVm.UserName);

                if (userModel != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userModel, UserVm.Password);

                    if (found)
                    {
                        List<Claim> Claims = new List<Claim>();
                        Claims.Add(new Claim("FullName", userModel.FullName));
                        await signInMAnager.SignInWithClaimsAsync
                            (userModel, UserVm.RememberMe, Claims);
                        // Retrieve the user's roles
                        var userRoles = await userManager.GetRolesAsync(userModel);

                        // Redirect based on roles
                        if (userRoles.Contains("Patient"))
                        {
                            Patient patient = context.Patients.FirstOrDefault(p => p.UserName == userModel.UserName);

                            // Store the patientid in a claim
                            var claims = new List<Claim>
                        {
                            new Claim("patientid", patient?.ID.ToString() ?? string.Empty)
                        };
                            await userManager.AddClaimsAsync(userModel, claims);
                            await signInMAnager.SignInAsync(userModel, UserVm.RememberMe);
                            return RedirectToAction("Details", "Patient", new { patientid = patient?.ID });

                        }
                        else if (userRoles.Contains("Admin"))
                        {
                            return RedirectToAction("Index", "Statistics");
                        }
                        else if (userRoles.Contains("Doctor"))
                        {
                            var doctorId = context.Staffs
                                            .Where(p => p.UserName == userModel.UserName)
                                            .Select(p => p.ID)
                                            .FirstOrDefault();
                            return RedirectToAction("DisplayAppointment", "Doctor", new { _docID = doctorId });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                ModelState.AddModelError("", "Username and password invalid");
            }

            return View(UserVm);
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel newUserVM)
        {
            Patient patient = new Patient();
            if (ModelState.IsValid)
            {
                // Check if the username already exists
                if (await userManager.FindByNameAsync(newUserVM.UserName) != null)
                {
                    ModelState.AddModelError("", "Username is already taken.");
                    return View(newUserVM);
                }
                //create account
                ApplicationUser userModel = new ApplicationUser
                {
                    FullName = newUserVM.FullName,
                    Gender = newUserVM.Gender,
                    Blood_Type = newUserVM.BloodType,
                    BDate = newUserVM.BDate,
                    PhoneNumber = newUserVM.PhoneNumber,
                    Country = newUserVM.Country,
                    Governorate = newUserVM.Governorate,
                    City = newUserVM.City,
                    UserName = newUserVM.UserName,
                    PasswordHash = userManager.PasswordHasher.HashPassword(null, newUserVM.Password),
                };
                
                patient.FullName = newUserVM.FullName;
                patient.Gender = newUserVM.Gender;
                patient.Country = newUserVM.Country;
                patient.Blood_Type = newUserVM.BloodType;
                patient.Governorate = newUserVM.Governorate;
                patient.City = newUserVM.City;
                patient.Password = userManager.PasswordHasher.HashPassword(null, newUserVM.Password);
                patient.NurseId = 1;
                patient.Phone = newUserVM.PhoneNumber;
                patient.BDate = newUserVM.BDate;
                patient.ID = GetNextPatientId();
                patient.UserName = newUserVM.UserName;
                IdentityResult result = await userManager.CreateAsync(userModel, newUserVM.Password);
                if (result.Succeeded == true)
                {
                    context.Patients.Add(patient);
                    await context.SaveChangesAsync(); // Use asynchronous SaveChanges
                    await userManager.AddToRoleAsync(userModel, "Patient");
                    //creat cookie
                    context.Patients.Add(patient);
                    await signInMAnager.SignInAsync(userModel, false);
          
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }
            return View(newUserVM);
        }
        private int GetNextPatientId()
        {
           // return max id and increment by 1
            return context.Patients.Max(p => p.ID) + 1;
        }
        public async Task<IActionResult> Logout()
        {
            await signInMAnager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }
}
