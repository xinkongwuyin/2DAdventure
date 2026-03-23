using System;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour, ISaveable
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;

    [Header("受伤无敌")]
    public float invulnerableDuration;
    private float invulnerableCount;
    public bool invulnerable;

    [Header("监听事件")]
    public VoidEventsSQ newGameEvent;

    public UnityEvent<Character>OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;

    private void Start()
    {

    }

    private void Update()
    {


            if (invulnerable)
            {
                invulnerableCount -= Time.deltaTime;
                if (invulnerableCount <= 0)
                {
                    invulnerable = false;
                }
            }
  
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += OnNewGame;
        ISaveable SAVEABLE = this;
        SAVEABLE.RegisterSaveData();
    }



    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= OnNewGame;
        ISaveable SAVEABLE = this;
        SAVEABLE.UnRegisterSaveData();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Water"))
        {
            if (currentHealth > 0)
            {
                currentHealth = 0;
                OnHealthChange?.Invoke(this);
                OnDie?.Invoke();
            }

        }
    }
    private void OnNewGame()
    {
        currentHealth = maxHealth;

        OnHealthChange?.Invoke(this);
    }

    public void TakeDamage(Attack attacker)
    { 

       if(invulnerable) return;
        // Debug.Log("受到攻击，损失生命值：" + attacker.damage);
        ;
        if (currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            // 受伤
            OnTakeDamage?.Invoke(attacker.transform);
            
        }
        else if(currentHealth!=0)
        {
            currentHealth = 0;
            //死亡
            OnDie?.Invoke();
        }

        OnHealthChange?.Invoke(this);
    }

    private void TriggerInvulnerable()
    {
        invulnerable = true;
        invulnerableCount = invulnerableDuration;
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetLoadData(Data data)
    {
        if(data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = transform.position;
            data.floatSaveData[GetDataID().ID + "health"] = this.currentHealth;
            data.floatSaveData[GetDataID().ID + "power"] = this.currentPower;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, transform.position);
            data.floatSaveData.Add(GetDataID().ID+"health", this.currentHealth);
            data.floatSaveData.Add(GetDataID().ID+"power", this.currentPower);

            OnHealthChange?.Invoke(this);
        }
    }

    public void LoadData(Data data)
    {
        if(data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID];

            this.currentHealth = data.floatSaveData[GetDataID().ID + "health"];
            this.currentPower = data.floatSaveData[GetDataID().ID + "power"];

            OnHealthChange?.Invoke(this);

        }
    }
}
