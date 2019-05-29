#include <Encoder.h>
#include <FastLED.h>

/* 
 * Encoder Pins
 * ----------- 
 * CLK --> D3
 * DT  --> D2
 * SW  --> D4
 * +   --> 5v
 * GND --> GND
 * 
 */

Encoder myEnc(2, 3);
const int swPin = 4 ;
long oldPosition  = -999;

/* LEDs */
#define LED_PIN     7
#define NUM_LEDS    150
CRGB leds[NUM_LEDS];

/* RGB Loop */
int cycle = 6;
int brightness = 0;
int maxBrightness = 130;
int r;
int g;
int b;

int direction = 0;
int changeSpeed = 1;
String sendString = "";

void setup() {
  
  r = maxBrightness;
  g = maxBrightness;
  b = 0;
  
  pinMode(swPin, INPUT);
  Serial.begin(9600);
  FastLED.addLeds<WS2812, LED_PIN, GRB>(leds, NUM_LEDS);


}

void loop() {
  // Read from rotary encoder.
  long newPosition = myEnc.read();

  if (newPosition != oldPosition) {
    
    if (newPosition > oldPosition) {
      direction = 1;
    } else {
      direction = -1;
    }

    oldPosition = newPosition;

    String newPositionString = String(newPosition);
    String directionString = String(direction);

//    sendString = "@dir=" + directionString + ",val=" + newPositionString + "@";
   
    Serial.println(newPosition);
    
    updateColor( );

  } else {
    direction = 0;
  }

  /* Will eventually move this inside the above if 
   * statement so the color shift is controlled by
   * a rotary encoder.  Down here for debugging. 
   */
//  delay(50);

}

void updateColor(  ) {

//  Serial.println(" "); // For readability. 

  if ( brightness >= maxBrightness ) {
    brightness = 1;
    
    if ( cycle < 6 ) {
      cycle++;
    } else {
      cycle = 1;
    }
    
  } else {
    brightness++;
  }

  if ( cycle == 1) {
    b = b + changeSpeed;
  }
  else if ( cycle == 2) { /* rb --> b */
    r = r - changeSpeed;
  }
  else if ( cycle == 3) { /* b --> bg */
    g = g + changeSpeed;
  }
  else if ( cycle == 4) { /* bg --> g */
    b = b - changeSpeed;
  }
  else if ( cycle == 5) { /* g --> rg */
    r = r + changeSpeed;
  }
  else if ( cycle == 6) { /* g --> rg */
    g = g - changeSpeed;
  }
  else {
    Serial.print("==================== ");
  }

//  Serial.print("brightness:");
//  Serial.print(brightness);
//  Serial.print("   cycle:");
//  Serial.println(cycle);
//  Serial.print("r:");
//  Serial.print(r);
//  Serial.print(" g:");
//  Serial.print(g); // goes completely bonkers
//  Serial.print(" b:");
//  Serial.print(b);
//  Serial.println(" ");
//  Serial.print("r:");
//  Serial.print(r, HEX);
//  Serial.print(" g:");
//  Serial.print(g, HEX); // goes completely bonkers
//  Serial.print(" b:");
//  Serial.print(b, HEX);
//  Serial.println(" ");

  uint8_t Ur = r;
  uint8_t Ug = g;
  uint8_t Ub = b;

  for (int i = 0; i < NUM_LEDS; i++) {
    leds[i].setRGB( Ur, Ug, Ub );
  }

  FastLED.show();

}
