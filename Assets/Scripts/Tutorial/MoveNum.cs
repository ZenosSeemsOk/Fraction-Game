using UnityEngine;

public class MoveNum : MonoBehaviour
{
    [SerializeField] GameObject num1;
    [SerializeField] GameObject num2;
    [SerializeField] Transform movePos1;
    [SerializeField] Transform movePos2;
    public LineDivider lineDivider;
    private GameManager gm;
    private musicManager mM;
    private void Start()
    {
        gm = GameManager.instance;
        mM = musicManager.Instance;
        if(gm.levelindex==0 && !gm.levelunlocked[0])
        {
            Invoke("Tween", .5f);
        }
        else
        {
            this.enabled = false;
        }
    }

    private void Tween()
    {
        num1.transform.GetChild(0).gameObject.SetActive(true);
        LeanTween.move(num1, movePos1, 2f).setDelay(1f).setOnComplete(() =>
        {
            mM.PlayOnceClip("number_Placed");
            num1.transform.GetChild(0).gameObject.SetActive(false);
            num2.transform.GetChild(0).gameObject.SetActive(true);
            lineDivider.impNumeValue = 1;
            LeanTween.move(num2, movePos2, 2f).setDelay(.5f).setOnComplete(() =>
            {
                mM.PlayOnceClip("number_Placed");
                num2.transform.GetChild(0).gameObject.SetActive(false);
                lineDivider.impDenoValue = 2;
            }
            );
        }
        );
    }
}
