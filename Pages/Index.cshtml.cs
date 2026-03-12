using Azure.AI.Vision.ImageAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace computervisionimageanalyser.Pages;

public class IndexModel : PageModel
{
    public class ImageCaptionModel
    {
        public IFormFile Image { get; set; }
        public string ImageData { get; set; }
        public string ImageCaption { get; set; }
    }
    [BindProperty]
    public ImageCaptionModel Input { get; set; }

    public IndexModel()
    {
        Input = new ImageCaptionModel();
    }
    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        string endpooint = "put endpoint here";
        string key = "put key here";
    
        ImageAnalysisClient client = new ImageAnalysisClient(new Uri(endpooint), new Azure.AzureKeyCredential(key));
        if (Input.Image != null)
        {
            using MemoryStream ms = new MemoryStream();
            await Input.Image.CopyToAsync(ms);
            ms.Position = 0;
            Input.ImageData = Convert.ToBase64String(ms.ToArray());

            ImageAnalysisResult imageAnaysisResult = await client.AnalyzeAsync(BinaryData.FromStream(ms),VisualFeatures.Caption, new
            ImageAnalysisOptions
            {
                GenderNeutralCaption = false
            });


            Input.ImageCaption = $" Caption: {imageAnaysisResult.Caption.Text} | Confidence: {imageAnaysisResult.Caption.Confidence}";

          ;
        }
          return Page();
    }
}
