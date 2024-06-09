using UnityEngine;
using TMPro;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject eggPrefab;
    public GameObject chickPrefab;
    public GameObject henPrefab;
    public GameObject roosterPrefab;

    public TextMeshProUGUI eggCountText;
    public TextMeshProUGUI chickCountText;
    public TextMeshProUGUI henCountText;
    public TextMeshProUGUI roosterCountText;

    private int eggCount = 0;
    private int chickCount = 0;
    private int henCount = 0;
    private int roosterCount = 0;

    void Start()
    {
        SpawnInitialEgg();
    }

    void SpawnInitialEgg()
    {
        Vector3 startPosition = new Vector3(654, 410, -152);  // Change position as needed
        GameObject initialEgg = Instantiate(eggPrefab, startPosition, Quaternion.identity);
        eggCount++;
        UpdateUI();

        StartCoroutine(HatchEggCoroutine(initialEgg, true));
    }

    IEnumerator HatchEggCoroutine(GameObject egg, bool guaranteedHen)
    {
        yield return new WaitForSeconds(10);

        HatchEgg(egg.transform.position, guaranteedHen);
        Destroy(egg);
    }

    public void HatchEgg(Vector3 position, bool guaranteedHen = false)
    {
        if (chickPrefab == null)
        {
            Debug.LogError("Chick Prefab is not assigned!");
            return;
        }
        if (eggCount > 0)
        {
            GameObject chick = Instantiate(chickPrefab, position, Quaternion.identity);
            eggCount = Mathf.Max(0, eggCount - 1);
            chickCount++;
            UpdateUI();

            StartCoroutine(GrowChickCoroutine(chick, guaranteedHen));
        }
    }

    IEnumerator GrowChickCoroutine(GameObject chick, bool guaranteedHen)
    {
        yield return new WaitForSeconds(10);

        if (guaranteedHen)
        {
            GrowToHen(chick.transform.position);
        }
        else
        {
            if (Random.value < 0.5f)
            {
                GrowToHen(chick.transform.position);
            }
            else
            {
                GrowToRooster(chick.transform.position);
            }
        }

        Destroy(chick);
    }

    public void GrowToHen(Vector3 position)
    {
        if (henPrefab == null)
        {
            Debug.LogError("Hen Prefab is not assigned!");
            return;
        }
        GameObject hen = Instantiate(henPrefab, position, Quaternion.identity);
        chickCount = Mathf.Max(0, chickCount - 1);
        henCount++;
        UpdateUI();

        StartCoroutine(HenLifeCycle(hen));
    }

    IEnumerator HenLifeCycle(GameObject hen)
    {
        yield return new WaitForSeconds(30);

        int numberOfEggs = Random.Range(2, 10); // Ensure minimum of 2 eggs and maximum of 10 eggs
        for (int i = 0; i < numberOfEggs; i++)
        {
            LayEgg(hen.transform.position);
        }

        yield return new WaitForSeconds(10);

        henCount = Mathf.Max(0, henCount - 1);
        UpdateUI();
        Destroy(hen);
    }

    public void GrowToRooster(Vector3 position)
    {
        if (roosterPrefab == null)
        {
            Debug.LogError("Rooster Prefab is not assigned!");
            return;
        }
        GameObject rooster = Instantiate(roosterPrefab, position, Quaternion.identity);
        chickCount = Mathf.Max(0, chickCount - 1);
        roosterCount++;
        UpdateUI();

        StartCoroutine(RoosterLifeCycle(rooster));
    }

    IEnumerator RoosterLifeCycle(GameObject rooster)
    {
        yield return new WaitForSeconds(40);

        roosterCount = Mathf.Max(0, roosterCount - 1);
        UpdateUI();
        Destroy(rooster);
    }

    public void LayEgg(Vector3 position)
    {
        if (eggPrefab == null)
        {
            Debug.LogError("Egg Prefab is not assigned!");
            return;
        }
        Vector3 eggPosition = position + new Vector3(Random.Range(-2f, 2f), 10, Random.Range(-2f, 2f));
        GameObject egg = Instantiate(eggPrefab, eggPosition, Quaternion.identity);
        eggCount++;
        UpdateUI();

        StartCoroutine(HatchEggCoroutine(egg, false));
    }

    void UpdateUI()
    {
        if (eggCountText != null)
        {
            eggCountText.text = "Eggs: " + eggCount;
        }
        else
        {
            Debug.LogError("Egg Count Text is not assigned!");
        }

        if (chickCountText != null)
        {
            chickCountText.text = "Chicks: " + chickCount;
        }
        else
        {
            Debug.LogError("Chick Count Text is not assigned!");
        }

        if (henCountText != null)
        {
            henCountText.text = "Hens: " + henCount;
        }
        else
        {
            Debug.LogError("Hen Count Text is not assigned!");
        }

        if (roosterCountText != null)
        {
            roosterCountText.text = "Roosters: " + roosterCount;
        }
        else
        {
            Debug.LogError("Rooster Count Text is not assigned!");
        }
    }
}
