﻿using UnityEngine;
using System.Collections;

public abstract class Mob : MonoBehaviour{
	
	public Stats stats = new Stats();
	private float stunTime = 0;
	private float canMove = 0;

	public Skill[] skills = new Skill[6];

	public void replaceSkill(int skillNum, Skill skill) {
		skills[skillNum] = skill;
	}

	public virtual Quaternion rotation {
		get {
			return gameObject.transform.rotation;
		}
		set {
			gameObject.transform.rotation = value;
		}
	}

	public virtual Transform feetTransform {
		get {
			return gameObject.transform;
		}
	}

	public virtual Vector3 position {
		get {
			return gameObject.transform.position;
		}
		set {
			gameObject.transform.position = value;
		}
	}

	public bool isStunned()
	{
		return stunTime >= Time.fixedTime;
	}
	
	public void addStunTime(float t)
	{
		if (isStunned ()) 
			stunTime += t;
		else
			stunTime = Time.fixedTime + t; 
	}
	
	public void setStunTime(float t)
	{
		if(getStunRemaining() < t)
			stunTime = Time.fixedTime + t;
	}
	
	public float getStunRemaining()
	{
		float output = stunTime - Time.fixedTime;
		if (output < 0)
			output = 0;
		return output; 
	}

	public bool hurt(float damage) {
		stats.health -= damage;
		return stats.health <= 0;
	}

	public bool useMana(float amount) {
		bool enoughMana = (Mathf.Ceil(stats.mana) >= amount);
		if (enoughMana)
			stats.mana -= amount;
		return enoughMana;
	}

	public abstract string getName();
	public abstract Vector3 getTargetLocation();
	void Start() {
		OnStart();
		stats.health = stats.maxHealth;
		stats.mana = stats.maxMana;
	}

	void Update() {
        if (stats.health <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }
		if(isStunned())
			return;
		OnUpdate();
		if (stats.health < stats.maxHealth)
			stats.health += stats.healthRegen * Time.deltaTime;
		else if (stats.health > stats.maxHealth)
			stats.health = stats.maxHealth;
		if (stats.mana < stats.maxMana)
			stats.mana += stats.manaRegen * Time.deltaTime;
		else if (stats.mana > stats.maxMana)
			stats.mana = stats.maxMana;
	}

    string[] dropTable = { "StrengthGem", "DexterityGem", "IntelGem" };

    void DropItem() {
        if (Random.Range(1, 101) <= 10) {
            GameObject Drop = Instantiate(Resources.Load<GameObject>(dropTable[Random.Range(0,3)]));
            Drop.transform.position = position;
        }

    }

	void FixedUpdate() {
		if(isStunned())
			return;
		if(canMove < Time.fixedTime)
			movement();
		OnFixedUpdate();
	}

	public void disableMovement(float time) {
		canMove = Time.fixedTime + time;
	}

	public float getCanMove() {
		return canMove;
	}

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        
    }

	public abstract void movement();
	public virtual void OnStart() {}
	public virtual void OnUpdate() {}
	public virtual void OnFixedUpdate() { }
}
