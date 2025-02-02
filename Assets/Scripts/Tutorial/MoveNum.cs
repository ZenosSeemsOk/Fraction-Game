using UnityEngine;

public class MoveNum : MonoBehaviour
{
    [SerializeField] GameObject num1;
    [SerializeField] GameObject num2;
    [SerializeField] Transform movePos1;
    [SerializeField] Transform movePos2;
    public LineDivider lineDivider;
    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
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
            num1.transform.GetChild(0).gameObject.SetActive(false);
            num2.transform.GetChild(0).gameObject.SetActive(true);
            lineDivider.impNumeValue = 1;
            LeanTween.move(num2, movePos2, 2f).setDelay(.5f).setOnComplete(() =>
            {
                num2.transform.GetChild(0).gameObject.SetActive(false);
                lineDivider.impDenoValue = 2;
            }
            );
        }
        );
    }
}
