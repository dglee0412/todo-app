namespace TodoApi.Dtos
{
    public record RegisterDto
    (
        string Email,
        string Password,
        string DisplayName
    );

    public record LoginDto
    (
        string Email,
        string Password
    );

    public record AuthResponseDto
    (
        int UserId,
        string Email,
        string DisplayName,
        string Token
    );
}
