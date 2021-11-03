using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InspectionAI.Users.Dto
{
    public class ChangeRefreshTimeDto
    {
        [Required]
        public int RefreshTime { get; set; }

    }
}
