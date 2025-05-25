export interface IFeedback{
    feedbackId : number,
    message : string,
    rating : number,
    submittedAt : Date,
    respondedAt : Date | null,
    responseMessage : string | null
}

export interface IPatientFeedback {
    message : string,
    rating : number
}

export interface IAdminResponse{
    feedbackId : number,
    responseMessage : string
}