using PatchGenerator.Helper;

namespace PatchGenerator.Data
{
    class PatchGeneratorConfig : Singleton<PatchGeneratorConfig>
    {
        public NationalSetting NationalConfig { get; set; }
        public PatchGeneratorConfig()
        {
            NationalConfig = new NationalSetting();
        }
    }
    public class NationalSetting
    {
        public e_nation_code NationCode { get; set; }
    }
}
