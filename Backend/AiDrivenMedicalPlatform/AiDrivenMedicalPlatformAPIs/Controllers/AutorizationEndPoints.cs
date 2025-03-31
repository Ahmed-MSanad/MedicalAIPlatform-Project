using Microsoft.AspNetCore.Authorization;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    public static class AutorizationEndPoints
    {
        public static IEndpointRouteBuilder MapAuthorizationEndPoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/AdminDashboard", AdminDashboard);
            app.MapGet("/PatientDashboard", PatientDashboard);
            app.MapGet("/HospitalMembersOnly", HospitalMembersOnly);
            app.MapGet("/FemaleOnly", ForFemaleOnly);
            app.MapGet("/ForFemaleDoctorOnly", ForFemaleDoctorOnly);
            app.MapGet("/Above18", Above18);
            return app;
        }
        [Authorize(Roles = "Admin")] // Besides that this endpoint is restricted to authenticated users only, this endpoint also is only accessible to users with the role "Admin" and the middleware will verify this and otherwize the request will get denied.
        private static string AdminDashboard()
        {
            return "Welcome to the Admin Dashboard!";
        }
        [Authorize(Roles = "Patient")]
        private static string PatientDashboard()
        {
            return "Welcome to the Patient Dashboard!";
        }
        [Authorize(Policy = "HasHospitalId")]
        private static string HospitalMembersOnly()
        {
            return "Welcome to the Hospital Members Dashboard!";
        }
        [Authorize(Policy = "FemaleOnly")]
        private static string ForFemaleOnly()
        {
            return "Welcome to For Female Only Request";
        }
        [Authorize(Roles = "Doctor", Policy = "FemaleOnly")]
        private static string ForFemaleDoctorOnly()
        {
            return "Welcome to For Female Doctor Only Request";
        }
        [Authorize(Policy = "Above18")]
        private static string Above18()
        {
            return "Welcome to For Above 18 Request";
        }
    }
}
