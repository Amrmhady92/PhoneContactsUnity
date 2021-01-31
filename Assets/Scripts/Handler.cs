using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler : MonoBehaviour
{
    private float clickCoolDown = 0.2f; 
    private static Handler instance;

    public static Handler Instance
    {
        get
        {
            return instance;
        }
    }

    private bool canClick = true;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void OnContactClicked(Contact contact)
    {
        if (canClick == false) return;

    }

    private void ClickCoolDown()
    {
        canClick = false;
        StartCoroutine(WaitThenDo(clickCoolDown, () => { canClick = true; }));
    }

    IEnumerator WaitThenDo(float waitTime, System.Action callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback?.Invoke();
    }
}
