using UnityEngine;

public class BowAnimator : MonoBehaviour
{
    [SerializeField] private AntiAircraftLauncher aAL;
    private Animator _animator;
    private const string BOW_PULLED = "bowPulled";
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_animator != null)
        {
            _animator.SetBool(BOW_PULLED,aAL.isHolding);
        }
        
    }

}
