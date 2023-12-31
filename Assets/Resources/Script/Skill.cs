using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
struct SkillInfo
{
    public Button skillBtn;
    public float coolDown;
    public float resetTime;
    public float buffTime;
    public float skillPower;
    public Text coolTime;
}

public class Skill : Singleton<Skill>
{
    [SerializeField] private List<SkillInfo> skill;
    Color normalColor;
    Color disableColor;

    void Start()
    {
        normalColor = skill[0].skillBtn.colors.normalColor;
        disableColor = skill[0].skillBtn.colors.disabledColor;

        for (int i = 0; i < skill.Count; ++i)
            skill[i].coolTime.text = skill[i].coolDown.ToString();
    }

    public void ClickSkill(string _name)
    {
        foreach (SkillInfo element in skill)
        {
            if (EventSystem.current.IsPointerOverGameObject() == element.skillBtn &&
                element.skillBtn.name == _name)
            {
                element.skillBtn.GetComponent<Image>().color = disableColor;
                element.skillBtn.enabled = false;
                StartCoroutine(RunningSkill(element.skillBtn, element.coolTime, element.skillPower,
                    element.coolDown, element.resetTime, element.buffTime));
                SkillName(element, element.skillPower);
            }
        }
    }

    void SkillName(SkillInfo _skill, float _power)
    {
        if (_skill.skillBtn.name == "PowerUp")
            GameManager.Instance.player.CurrentAtk(_power);
        else if (_skill.skillBtn.name == "Thunder")
        {
            GameManager.Instance.thunder.SummonThunder(GameManager.Instance.thunder.gameObject, 
                GameManager.Instance.thunderPos, _skill.skillPower);
        }
    }

    IEnumerator RunningSkill(Button _Obj, Text _timeText, float _power, float _time, float resetTime, float _buffTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.0f);

        while (_Obj.enabled == false)
        {
            if (_buffTime > 0 && _Obj.name == "Heal") 
                GameManager.Instance.player.CurrentHp(_power);

            _timeText.gameObject.SetActive(true);
            yield return waitForSeconds;

            _buffTime--;
            _timeText.text = _time > 0 ? ((int)--_time).ToString() : 0.ToString();

            if (_buffTime == 0)
            {
                if (_Obj.name == "PowerUp") 
                    GameManager.Instance.player.CurrentAtk(-_power);
            }

            if (_time == 0)
            {
                Coroutine runningCoroutine = StartCoroutine(RunningSkill(_Obj, _timeText, _power, _time, resetTime, _buffTime));
                _timeText.text = resetTime.ToString();
                _Obj.gameObject.GetComponent<Image>().color = normalColor;
                _Obj.enabled = true;
                _timeText.gameObject.SetActive(false);
                StopCoroutine(runningCoroutine);
            }
        }
    }
}
