using UnityEngine;

public class PrivacyController : MonoBehaviour
{
    private string privacyPolicyURL = "https://kill-the-plague.flycricket.io/privacy.html";
    private string termsPolicyURL = "https://kill-the-plague.flycricket.io/terms.html";

    public void OpenPrivacy()
    {
        Application.OpenURL(privacyPolicyURL);
    }

    public void OpenTerms()
    {
        Application.OpenURL(termsPolicyURL);
    }
}
