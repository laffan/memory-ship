/* Encoder Library - Basic Example
 * http://www.pjrc.com/teensy/td_libs_Encoder.html
 *
 * This example code is in the public domain.
 */

#include <Encoder.h>

Encoder myEnc(2, 3);
const int swPin= 4 ;

void setup() {
  Serial.begin(9600);
  pinMode(swPin, INPUT);

}

long oldPosition  = -999;

void loop() {
  long newPosition = myEnc.read();

  if (newPosition != oldPosition) {
    oldPosition = newPosition;
    Serial.println(newPosition);
  }


}
