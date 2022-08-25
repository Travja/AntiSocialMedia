using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AntiData.Model;

public class AntiUser : IdentityUser
{
    public string Avatar { get; set; } =
        "https://lh3.googleusercontent.com/b91FFh2EPsExNTHHqECbEQsqDSgaBeOxYWIZfNeYdXfmBOIFPpbyB2VphB_6m_g5iu_ACtgA11X-64TsqWUtdv5x9fFzco4N7OzFYio=w600";
}