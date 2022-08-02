using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HogwartsPotions.Models.Entities;

public class Room
{
    public Room()
    {
        Residents = new();
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public int Capacity { get; set; }

    public HashSet<Student> Residents { get; set; }
}