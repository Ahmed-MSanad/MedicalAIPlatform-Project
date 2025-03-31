export const claimReq = {
    adminOnly : ((claim : any) => claim.role === "Admin"),
    doctorOnly : ((claim : any) => claim.role === "Doctor"),
    patientOnly : ((claim : any) => claim.role === "Patient"),
}