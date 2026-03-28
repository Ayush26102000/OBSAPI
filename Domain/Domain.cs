namespace Domain;

public class Clinic
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}

public class Service
{
    public Guid Id { get; set; }
    public Guid ClinicId { get; set; }
    public string Name { get; set; }
    public int DurationMinutes { get; set; }
}

public class Booking
{
    public Guid Id { get; set; }
    public Guid ClinicId { get; set; }
    public Guid ServiceId { get; set; }
    public string PatientName { get; set; }
    public string Phone { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; }
}

public class Availability
{
    public Guid Id { get; set; }
    public Guid ClinicId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

public class BlockedSlot
{
    public Guid Id { get; set; }
    public Guid ClinicId { get; set; }
    public DateTime BlockedDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}