using UnityEngine;
using UnityEngine.UI;

public class IntroSplash : MonoBehaviour
{
	public Text TipText;

	private string[] Tips = new string[42]
	{
		"Mayrotastic Kompani!", "Luggy?.", "Randomh Spaslh Teksxt!!", "Brign it onh!", "Letsa Go!!", "Wah Wah Wah!", "Hapi Biday Mayro!", "Jummy andh noice!.", "WAAAAAAAAAAH!", "Luggy iz in de kastel koutrjard!",
		"9 + 10 = 21", "All Toasters Toast Toast!", "Interactive Media!", "So much fun!", "Wayro?", "Is the best!", "Full Metal Mayro!", "Where's Waluggy?", "Mayro Mowee!", "Noice!",
		"Happi Birfday To Yu!", "Gusvenga!", "The Klassik!", "No Question!", "I never doubted it!", "You cannot beat me!", "Speen!", "I Wonder What's for Dinner!", "Hav a Noice Day!", "Fsave da Princes!",
		"Vinny Plaiyed dis!", "Best Gaemz Kompani!", "Lotsa Spaghetti!", "When Update?", "Kuality Gaemz!", "Yez!", "Squadala!", "Fantaztik!", "De Ultimeyt Kompani!", "Sampel Teksd!",
		"Enjoay!", "Goodie Gaems Kompani!"
	};

	public RawImage TipImage;

	public Texture[] TipTexture;

	public void Start()
	{
		int num = Mathf.RoundToInt(Random.Range(0f, 29f));
		TipText.text = Tips[num];
	}

	public void Update()
	{
	}

	public void PutTip()
	{
	}
}
