import { ENotificationType } from "../Enums/enotification-type";

export interface INotification {
    id : number,
    message : string,
    from : string,
    to : string,
    subject : string,
    type : ENotificationType,
    submittedAt: string
}
