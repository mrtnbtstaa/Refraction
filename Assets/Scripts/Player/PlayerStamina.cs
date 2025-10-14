using System.Collections;
using UnityEngine;

public class PlayerStamina
{
    public PlayerProperties playerProperties;
    private readonly float maxStamina = 100f;
    public PlayerStamina(PlayerProperties playerProperties) => this.playerProperties = playerProperties;
    public IEnumerator IncreaseStamina(float recoveryRate, System.Action<float> onResult)
    {
        yield return new WaitForSeconds(1f);

        while (playerProperties.stamina < maxStamina)
        {
            // Increase stamina overtime
            playerProperties.stamina += recoveryRate * Time.deltaTime;
            // Clamp the stamina value
            playerProperties.stamina = Mathf.Min(playerProperties.stamina, maxStamina);
            // Update UI in real time
            onResult?.Invoke(playerProperties.stamina);
            yield return null;
        }
        // to ensure final update is applied
        onResult?.Invoke(playerProperties.stamina);
    }
    public void Consume(float consumeAmount)
    {
        if (playerProperties.stamina < 0) return;
        playerProperties.stamina -= consumeAmount;
        playerProperties.stamina = Mathf.Max(playerProperties.stamina, 0f);
    }
    public bool IsStaminaFull()
    {
        playerProperties.stamina = Mathf.Min(playerProperties.stamina, maxStamina);
        return playerProperties.stamina >= maxStamina;
    }
    public bool CanPerformAction(float requiredStamina) => playerProperties.stamina >= requiredStamina;

}
