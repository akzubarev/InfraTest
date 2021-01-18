using UnityEngine;
using UnityEngine.UI;

public class FighterUpdater : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private Text name;

    [SerializeField]
    private InputField health, 
                       range,
                       damage, 
                       speed;

    // Start is called before the first frame update
    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        UpdateUI();
    }

    /// <summary>
    /// Обновляет фактическую информацию о выбранном бойце
    /// </summary>
    public void UpdateFighter()
    {
        Fighter fighter = null;
        int d = ParseText(damage.text, 1, 100, 10),
            h = ParseText(health.text, 1, 1000, 100),
            r = ParseText(range.text, 1, 1000, 10),
            s = ParseText(speed.text, 1, 100, 30);

        fighter = gameController.GetFighter(name.text).GetComponent<Fighter>();

        fighter.Damage = d;
        fighter.Health = h;
        fighter.Range = r;
        fighter.Speed = s;

        UpdateUI();
    }


    /// <summary>
    /// Обновляет выводимую информацию о выбранном бойце
    /// </summary>
    public void UpdateUI()
    {
        Fighter fighter = gameController.GetFighter(name.text);
        damage.text = fighter.Damage.ToString();
        health.text = fighter.Health.ToString();
        range.text = fighter.Range.ToString();
        speed.text = fighter.Speed.ToString();
    }


    /// <summary>
    /// Парсит число из полей ввода в инт
    /// </summary>
    /// <param name="text">текст из поля ввода</param>
    /// <param name="leftbound">левая граница</param>
    /// <param name="rightbound">правая граница</param>
    /// <param name="defaultvalue">значение по умолчанию, выставляется если значение 
    /// не удовлетворяет границам или инту</param>
    /// <returns>Спаршенное число</returns>
    public int ParseText(string text, int leftbound, int rightbound, int defaultvalue)
    {
        int result = -1;
        if (int.TryParse(text, out result))
            if (result >= leftbound && result <= rightbound)
                return result;

        return defaultvalue;
    }
}
