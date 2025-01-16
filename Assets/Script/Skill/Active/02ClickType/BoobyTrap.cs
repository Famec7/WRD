using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoobyTrap : ClickTypeSkill
{
    [SerializeField]
    private GameObject _trapPrefab;
    private GameObject _trapObject = null;
    public override void OnActiveEnter()
    {
        if (_trapObject == null)
            _trapObject = Instantiate(_trapPrefab);

        _trapObject.GetComponent<KunaiTrap>().Init(Data.GetValue(0));
        _trapObject.transform.position = ClickPosition;
        _trapObject.SetActive(true);
        
        StartCoroutine(DisableTrapAfterDelay());

    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {

    }

    IEnumerator DisableTrapAfterDelay ()
    {
        yield return new WaitForSeconds(3f);
        _trapObject.SetActive(false);
    }
}
