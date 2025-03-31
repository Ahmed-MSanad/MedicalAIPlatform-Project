import { Gender } from "../Enums/gender";

export interface UserProfile {
    fullName : string,
    dateOfBirth : Date,
    gender : Gender,
    address : string,
    occupation : string,
    emergencyContactName : string,
    emergencyContactNumber : string,
    familyMedicalHistory : string,
    pastMedicalHistory : string,
}
