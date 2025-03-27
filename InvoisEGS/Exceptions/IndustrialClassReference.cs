namespace InvoisEGS.Exceptions
{
    public static class IndustrialClassReference
    {
        private static readonly HashSet<string> IndustrialClass = new()
        {
            "00000","01111", "01112", "01113", "01119", "01120", "01131", "01132", "01133", "01134", "01135", "01136", "01137", "01138", "01140", "01150", "01160", "01191", "01192", "01193", "01199", "01210", "01221", "01222", "01223", "01224", "01225", "01226", "01227", "01228", "01229", "01231", "01232", "01233", "01239", "01241", "01249", "01251", "01252", "01253", "01259", "01261", "01262", "01263", "01269", "01271", "01272", "01273", "01279", "01281", "01282", "01283", "01284", "01285", "01289", "01291", "01292", "01293", "01294", "01295", "01296", "01299", "01301", "01302", "01303", "01304", "01411", "01412", "01413", "01420", "01430", "01441", "01442", "01443", "01450", "01461", "01462", "01463", "01464", "01465", "01466", "01467", "01468", "01469", "01491", "01492", "01493", "01494", "01495", "01496", "01497", "01499", "01500", "01610", "01620", "01631", "01632", "01633", "01634", "01640", "01701", "01702", "02101", "02102", "02103", "02104", "02105", "02201", "02202", "02203", "02204", "02301", "02302", "02303", "02309", "02401", "02402", "03111", "03112", "03113", "03114", "03115", "03119", "03121", "03122", "03123", "03124", "03129", "03211", "03212", "03213", "03214", "03215", "03216", "03217", "03218", "03219", "03221", "03222", "03223", "03224", "03225", "03229", "05100", "05200", "06101", "06102", "06103", "06104", "06201", "06202", "06203", "06204", "06205", "07101", "07102", "07210", "07291", "07292", "07293", "07294", "07295", "07296", "07297", "07298", "07299", "08101", "08102", "08103", "08104", "08105", "08106", "08107", "08108", "08109", "08911", "08912", "08913", "08914", "08915", "08916", "08917", "08918", "08921", "08922", "08923", "08931", "08932", "08933", "08991", "08992", "08993", "08994", "08995", "08996", "08999", "09101", "09102", "09900", "10101", "10102", "10103", "10104", "10109", "10201", "10202", "10203", "10204", "10205", "10301", "10302", "10303", "10304", "10305", "10306", "10401", "10402", "10403", "10404", "10405", "10406", "10407", "10501", "10502", "10509", "10611", "10612", "10613", "10619", "10621", "10622", "10623", "10711", "10712", "10713", "10714", "10721", "10722", "10731", "10732", "10733", "10741", "10742", "10750", "10791", "10792", "10793", "10794", "10795", "10799", "10800", "11010", "11020", "11030", "11041", "11042", "12000", "13110", "13120", "13131", "13132", "13139", "13910", "13921", "13922", "13930", "13940", "13990", "14101", "14102", "14103", "14109", "14200", "14300", "15110", "15120", "15201", "15202", "15203", "15209", "16100", "16211", "16212", "16221", "16222", "16230", "16291", "16292", "17010", "17020", "17091", "17092", "17093", "17094", "17099", "18110", "18120", "18200", "19100", "19201", "19202", "20111", "20112", "20113", "20119", "20121", "20129", "20131", "20132", "20133", "20210", "20221", "20222", "20231", "20232", "20291", "20292", "20299", "20300", "21001", "21002", "21003", "21004", "21005", "21006", "21007", "21009", "22111", "22112", "22191", "22192", "22193", "22199", "22201", "22202", "22203", "22204", "22205", "22209", "23101", "23102", "23109", "23911", "23912", "23921", "23929", "23930", "23941", "23942", "23951", "23952", "23953", "23959", "23960", "23990", "24101", "24102", "24103", "24104", "24109", "24201", "24202", "24209", "24311", "24312", "24320", "25111", "25112", "25113", "25119", "25120", "25130", "25200", "25910", "25920", "25930", "25991", "25992", "25993", "25994", "25999", "26101", "26102", "26103", "26104", "26105", "26109", "26201", "26202", "26300", "26400", "26511", "26512", "26520", "26600", "26701", "26702", "26800", "27101", "27102", "27200", "27310", "27320", "27330", "27400", "27500", "27900", "28110", "28120", "28130", "28140", "28150", "28160", "28170", "28180", "28191", "28192", "28199", "28210", "28220", "28230", "28240", "28250", "28260", "28290", "29101", "29102", "29200", "29300", "30110", "30120", "30200", "30300", "30400", "30910", "30920", "30990", "31001", "31002", "31003", "31009", "32110", "32120", "32200", "32300", "32400", "32500", "32901", "32909", "33110", "33120", "33131", "33132", "33133", "33140", "33150", "33190", "33200", "35101", "35102", "35201", "35202", "35203", "35301", "35302", "35303", "36001", "36002", "37000", "38111", "38112", "38113", "38114", "38115", "38121", "38122", "38210", "38220", "38301", "38302", "38303", "38304", "38309", "39000", "41001", "41002", "41003", "41009", "42101", "42102", "42103", "42104", "42105", "42106", "42109", "42201", "42202", "42203", "42204", "42205", "42206", "42207", "42209", "42901", "42902", "42903", "42904", "42905", "42906", "42909", "43110", "43121", "43122", "43123", "43124", "43125", "43126", "43129", "43211", "43212", "43213", "43214", "43215", "43216", "43219", "43221", "43222", "43223", "43224", "43225", "43226", "43227", "43228", "43229", "43291", "43292", "43293", "43294", "43295", "43299", "43301", "43302", "43303", "43304", "43305", "43306", "43307", "43309", "43901", "43902", "43903", "43904", "43905", "43906", "43907", "43909", "45101", "45102", "45103", "45104", "45105", "45106", "45109", "45201", "45202", "45203", "45204", "45205", "45300", "45401", "45402", "45403", "46100", "46201", "46202", "46203", "46204", "46205", "46209", "46311", "46312", "46313", "46314", "46319", "46321", "46322", "46323", "46324", "46325", "46326", "46327", "46329", "46411", "46412", "46413", "46414", "46415", "46416", "46417", "46419", "46421", "46422", "46431", "46432", "46433", "46434", "46441", "46442", "46443", "46444", "46491", "46492", "46493", "46494", "46495", "46496", "46497", "46499", "46510", "46521", "46522", "46531", "46532", "46591", "46592", "46593", "46594", "46595", "46596", "46599", "46611", "46612", "46619", "46621", "46622", "46631", "46632", "46633", "46634", "46635", "46636", "46637", "46639", "46691", "46692", "46693", "46694", "46695", "46696", "46697", "46698", "46699", "46901", "46902", "46909", "47111", "47112", "47113", "47114", "47191", "47192", "47193", "47194", "47199", "47211", "47212", "47213", "47214", "47215", "47216", "47217", "47219", "47221", "47222", "47230", "47300", "47411", "47412", "47413", "47420", "47510", "47520", "47531", "47532", "47533", "47591", "47592", "47593", "47594", "47595", "47596", "47597", "47598", "47611", "47612", "47620", "47631", "47632", "47633", "47634", "47635", "47640", "47711", "47712", "47713", "47721", "47722", "47731", "47732", "47733", "47734", "47735", "47736", "47737", "47738", "47739", "47741", "47742", "47743", "47744", "47749", "47810", "47820", "47891", "47892", "47893", "47894", "47895", "47911", "47912", "47913", "47914", "47991", "47992", "47999", "49110", "49120", "49211", "49212", "49221", "49222", "49223", "49224", "49225", "49229", "49230", "49300", "50111", "50112", "50113", "50121", "50122", "50211", "50212", "50220", "51101", "51102", "51103", "51201", "51202", "51203", "52100", "52211", "52212", "52213", "52214", "52219", "52221", "52222", "52229", "52231", "52232", "52233", "52234", "52239", "52241", "52249", "52291", "52292", "52299", "53100", "53200", "55101", "55102", "55103", "55104", "55105", "55106", "55107", "55108", "55109", "55200", "55900", "56101", "56102", "56103", "56104", "56105", "56106", "56107", "56210", "56290", "56301", "56302", "56303", "56304", "56309", "58110", "58120", "58130", "58190", "58201", "58202", "58203", "59110", "59120", "59130", "59140", "59200", "60100", "60200", "61101", "61102", "61201", "61202", "61300", "61901", "61902", "61903", "61904", "61905", "61909", "62010", "62021", "62022", "62091", "62099", "63111", "63112", "63120", "63910", "63990", "64110", "64191", "64192", "64193", "64194", "64195", "64199", "64200", "64301", "64302", "64303", "64304", "64309", "64910", "64921", "64922", "64923", "64924", "64925", "64929", "64991", "64992", "64993", "64999", "65111", "65112", "65121", "65122", "65123", "65124", "65125", "65201", "65202", "65203", "65204", "65205", "65206", "65207", "65301", "65302", "66111", "66112", "66113", "66114", "66119", "66121", "66122", "66123", "66124", "66125", "66129", "66191", "66192", "66199", "66211", "66212", "66221", "66222", "66223", "66224", "66290", "66301", "66302", "66303", "68101", "68102", "68103", "68104", "68109", "68201", "68202", "68203", "68209", "69100", "69200", "70100", "70201", "70202", "70203", "70209", "71101", "71102", "71103", "71109", "71200", "72101", "72102", "72103", "72104", "72105", "72106", "72109", "72201", "72202", "72209", "73100", "73200", "74101", "74102", "74103", "74109", "74200", "74901", "74902", "74903", "74904", "74905", "74909", "75000", "77101", "77102", "77211", "77212", "77213", "77219", "77220", "77291", "77292", "77293", "77294", "77295", "77296", "77297", "77299", "77301", "77302", "77303", "77304", "77305", "77306", "77307", "77309", "77400", "78100", "78200", "78300", "79110", "79120", "79900", "80100", "80200", "80300", "81100", "81210", "81291", "81292", "81293", "81294", "81295", "81296", "81297", "81299", "81300", "82110", "82191", "82192", "82193", "82194", "82195", "82196", "82199", "82200", "82301", "82302", "82910", "82920", "82990", "84111", "84112", "84121", "84122", "84123", "84124", "84125", "84126", "84129", "84131", "84132", "84133", "84134", "84135", "84136", "84137", "84138", "84139", "84210", "84220", "84231", "84232", "84233", "84234", "84235", "84236", "84239", "84300", "85101", "85102", "85103", "85104", "85211", "85212", "85221", "85222", "85301", "85302", "85411", "85412", "85419", "85421", "85429", "85491", "85492", "85493", "85494", "85499", "85500", "86101", "86102", "86201", "86202", "86203", "86901", "86902", "86903", "86904", "86905", "86906", "86909", "87101", "87102", "87103", "87201", "87209", "87300", "87901", "87902", "87909", "88101", "88109", "88901", "88902", "88909", "90001", "90002", "90003", "90004", "90005", "90006", "90007", "90009", "91011", "91012", "91021", "91022", "91031", "91032", "92000", "93111", "93112", "93113", "93114", "93115", "93116", "93117", "93118", "93119", "93120", "93191", "93192", "93193", "93199", "93210", "93291", "93292", "93293", "93294", "93295", "93296", "93297", "93299", "94110", "94120", "94200", "94910", "94920", "94990", "95111", "95112", "95113", "95121", "95122", "95123", "95124", "95125", "95126", "95127", "95211", "95212", "95213", "95214", "95221", "95222", "95230", "95240", "95291", "95292", "95293", "95294", "95295", "95296", "95299", "96011", "96012", "96013", "96014", "96020", "96031", "96032", "96033", "96034", "96035", "96091", "96092", "96093", "96094", "96095", "96096", "96097", "96099", "97000", "98100", "98200", "99000"
        };
        public static bool ValidateIndustrialClass(string code)
        {
            string normalizedCode = code?.ToUpperInvariant() ?? string.Empty;
            bool isValid = IndustrialClass.Contains(normalizedCode);
            return isValid;
        }
    }
}