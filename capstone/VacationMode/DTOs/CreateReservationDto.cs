namespace VacationMode.DTOs;

public class CreateReservationDto
{
    public int PropertyId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}
