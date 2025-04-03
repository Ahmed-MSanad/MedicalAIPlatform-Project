import { Gender } from "../Enums/gender";

export interface UserProfile {
    email : string
    fullName : string,
    dateOfBirth : Date,
    gender : Gender,
    address : string,
    occupation : string,
    emergencyContactName : string,
    emergencyContactNumber : string,
    familyMedicalHistory : string,
    pastMedicalHistory : string,
    phone: string,
    imagePath: string
}
