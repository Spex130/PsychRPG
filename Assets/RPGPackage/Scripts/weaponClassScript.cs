using System;
using UnityEngine;

	public class weaponClassScript : MonoBehaviour
	{
		public string specialEffect; //magic, fire, poison, confusion, ice, slow
		public int attackDamage; 
		public int weaponHealth; //weapon degrades over time
		public int weaponSize;	//weight of weapon
		public string weaponName; //name of Weapon
		public float criticalHitChance; //chance of critical hit to maximize damage
		public string characterStatusChange;
		public string userAddition; //speed enhanced, strength enhance, health gain, 
		public int userAdditionValue;
		public string weaponStatus;


		public weaponClassScript (string weaponStatus, string characterStatusChange, int userAdditionValue, string specialEffect, int attackDamage,
		                          int weaponHealth, int weaponSize, string weaponName, float criticalHitChance, string userAddition )
		{
			//generic example
			//specialEffect = "Magic +3";
			//attackDamage = 20;
			//weaponHealth = 100;
			//weaponSize = normal;
			//weaponName = "Valkyrie Bow";
			//criticalHitChance = 0.1f;
			//userAddition = "Magic up";
			this.specialEffect = specialEffect;
			this.attackDamage = attackDamage;
			this.weaponHealth = 100;
			this.weaponSize = weaponSize;
			this.weaponName = weaponName;
			this.criticalHitChance = criticalHitChance;
			this.userAddition = userAddition; //either mag or whatevers
			this.userAdditionValue = userAdditionValue;
			this.characterStatusChange = characterStatusChange;
			this.weaponStatus = weaponStatus;


		}
		public void useWeapon(enemyClass enemy, characterScript character) {
			//improve character attributes
			if (userAddition == "mag") {
				character.mag = character.mag + userAdditionValue;
			}
			if (userAddition == "pDef") {
				character.pDef = character.pDef + userAdditionValue;
			}
			if (userAddition == "spd") {
				character.spd = character.spd + userAdditionValue;
			}
			if (userAddition == "dex") {
				character.dex = character.dex + userAdditionValue;
			}
			if (userAddition == "mdef") {
				character.mDef = character.mDef + userAdditionValue;
			}
			if (userAddition == "atk") {
				character.atk = character.atk + userAdditionValue;
			}
			character.status = characterStatusChange;//shows current addition to status "Magic UP + 5"
			System.Random r = new System.Random ();
			float num = r.Next (0, 1);
			//critical attack chance
			if (num == this.criticalHitChance) {
				this.attackDamage = attackDamage * 5;
			}

			//character attack is added with weapon attack and enemies health is decreased
			character.atk = character.atk + attackDamage;
			enemy.hp = enemy.hp - character.atk;


			//checking the repair status of weapon
			float newNum = r.Next(0, 20);
			if (newNum == 3) {
				this.weaponHealth -= 20;
			}
			if (this.weaponHealth == 100) {
				this.weaponStatus = "Perfect Health!";
			}
			if (this.weaponHealth < 100 && this.weaponHealth >= 30) {
				this.weaponStatus =  "Average Health!";
			}
			if (this.weaponHealth < 30) {
				this.weaponStatus = "Repair Soon!";
			}
			if (this.weaponHealth <= 0) {
				this.weaponHealth = 0;
				this.weaponStatus = "Broken!";
				this.attackDamage = 0;
			}




		}



	}


