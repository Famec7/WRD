using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoobyTrap : ClickTypeSkill
{
    [SerializeField]
    private GameObject _trapPrefab;
    private GameObject _trapObject = null;
    protected override void OnActiveEnter()
    {
        if (_trapObject == null)
            _trapObject = Instantiate(_trapPrefab);

        _trapObject.GetComponent<KunaiTrap>().Init(Data.GetValue(0));
        _trapObject.transform.position = clickPosition;
        _trapObject.SetActive(true);
        
        StartCoroutine(DisableTrapAfterDelay());

    }

    protected override INode.ENodeState OnActiveExecute()
    {
        IsActive = false;
        return INode.ENodeState.Success;
    }

    protected override void OnActiveExit()
    {

    }

    IEnumerator DisableTrapAfterDelay ()
    {
        yield return new WaitForSeconds(3f);
        _trapObject.SetActive(false);
    }
}
