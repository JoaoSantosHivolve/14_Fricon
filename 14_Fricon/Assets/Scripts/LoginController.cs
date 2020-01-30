using System.Collections;
using TMPro;
using UnityEngine;

public class LoginController : MonoBehaviour
{
    public Animator loginAnimator;
    public Animator loadingIcon;
    public Animator tocAnimator;

    public TMP_InputField nameField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI resultInfo;

    private const string FakeId = "admin";
    private const string FakePassword = "admin";


    public void CheckResults_OnClick()
    {
        loadingIcon.Play("LI_Loading");
        loginAnimator.Play("LOGIN_Loading");
        resultInfo.text = "";

        StartCoroutine(CheckValues());
    }

    private IEnumerator CheckValues()
    {
        yield return new WaitForSeconds(1f);

        if (nameField.text == FakeId && passwordField.text == FakePassword
            //DEBUG
            || (nameField.text == "" && passwordField.text == ""))
        {
            // Info matches
            loadingIcon.Play("LI_Off");
            passwordField.text = "";
            tocAnimator.Play("TOC_Enter");
            loginAnimator.Play("LOGIN_Exit");
        }
        else
        {
            // Info doesn't match
            loadingIcon.Play("LI_Off");
            passwordField.text = "";
            loginAnimator.Play("LOGIN_Waiting");
            resultInfo.text = "*wrong password or username \nDEBUG: username: admin | password: admin";
        }
    }
}
