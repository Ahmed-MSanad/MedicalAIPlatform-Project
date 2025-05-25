//using System.Text.Json;
//using System.Text.Json.Serialization;
//using AiDrivenMedicalPlatformAPIs.Controllers;

//namespace MedicalProj.Data.Models
//{
//    public class UserRegistrationModelConverter : JsonConverter<UserRegistrationModel>
//    {
//        public override UserRegistrationModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            using var jsonDoc = JsonDocument.ParseValue(ref reader);
//            var root = jsonDoc.RootElement;

//            if (!root.TryGetProperty("role", out var roleElement))
//            {
//                throw new JsonException("Role property is missing.");
//            }

//            var role = roleElement.GetString()?.ToLowerInvariant();

//            return role switch
//            {
//                "patient" => JsonSerializer.Deserialize<PatientRegistrationModel>(root.GetRawText(), options),
//                "doctor" => JsonSerializer.Deserialize<DoctorRegistrationModel>(root.GetRawText(), options),
//                "admin" => JsonSerializer.Deserialize<AdminRegistrationModel>(root.GetRawText(), options),
//                _ => throw new JsonException($"Invalid role: {role}")
//            };
//        }

//        public override void Write(Utf8JsonWriter writer, UserRegistrationModel value, JsonSerializerOptions options)
//        {
//            JsonSerializer.Serialize(writer, value, value.GetType(), options);
//        }
//    }
//}