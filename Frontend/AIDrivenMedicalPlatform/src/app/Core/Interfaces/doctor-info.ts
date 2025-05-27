export interface DoctorInfo {
  fullName: string
  specialisation: string
  workPlace: string
  fee: number
  rate: number
  image: string
  doctorSchedule: DoctorSchedule[]
}

export interface DoctorSchedule {
  day: string
  from: string
  to: string
}
