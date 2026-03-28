namespace Domain.DTOs;

public class CreateBookingDto
{
    public Guid ClinicId { get; set; }
    public Guid ServiceId { get; set; }
    public string PatientName { get; set; }
    public string Phone { get; set; }
    public DateTime AppointmentDate { get; set; }
}