import { EmojiConvertor } from "emoji-js";

class EmojiService {
  convertColonsToHtml(colons){    
    var emoji = new EmojiConvertor();
    emoji.img_sets.google.path =
      "https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/";
    emoji.img_sets.google.sheet =
      "https://unpkg.com/emoji-datasource-google@4.0.4/img/google/sheets-256/64.png";
    emoji.img_set = "google";
    emoji.use_sheet = true;

    return emoji.replace_colons(colons);
  }
}

export default EmojiService;
