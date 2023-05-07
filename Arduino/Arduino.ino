#include "rgb_lcd.h"

rgb_lcd lcd;

void setup()
{
  lcd.begin(16, 2);
  Serial.begin(9600);
}

void loop()
{
  if (Serial.available() > 0)
  {
  
    String serialMessage = Serial.readStringUntil('\n');
    if(serialMessage != ""){
    String memUsage = serialMessage.substring(serialMessage.indexOf("]") + 1, serialMessage.length()); 
    String cpuUsage = serialMessage.substring(0, serialMessage.indexOf(";"));
    String cpuClock = serialMessage.substring(serialMessage.indexOf(";") + 1, serialMessage.indexOf("["));
    String diskUsage = serialMessage.substring(serialMessage.indexOf("[") + 1, serialMessage.indexOf("]"));
    if (cpuUsage.toInt() > 40) {
      lcd.setRGB(255, 0, 0);
    } else if (cpuUsage.toInt() > 20 ) {
      lcd.setRGB(255, 255, 0);
    } else {
      lcd.setRGB(255, 255, 255);
    }
    lcd.clear();
    lcd.setCursor(0, 0);
    lcd.print("CPU:" + cpuUsage + "% " + cpuClock + "MHz");
    lcd.setCursor(0, 1);
    lcd.print("RAM:" + memUsage + "% " + "HD: " + diskUsage + "%");
    }
  }
}