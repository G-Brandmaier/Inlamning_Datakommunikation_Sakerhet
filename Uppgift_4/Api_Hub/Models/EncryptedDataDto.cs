using System.ComponentModel.DataAnnotations;

namespace Api_Hub.Models;

/// <summary>
/// Excpected structure when data is encrypted and send to the hub for decryption.
/// </summary>
public class EncryptedDataDto
{
    [Required]
    public byte[] Data { get; set; } = null!;
    [Required]
    public byte[] Iv { get; set; } = null!;
}
