
// Adjust your pin asignment as required
#define Player1 7
#define Player2 13

// timeout in ms before toggling the pin back low
#define DefaultTimeout 500


void setup() {
  pinMode(Player1, OUTPUT); 
  pinMode(Player1, OUTPUT);
  // Set them low!
  digitalWrite(Player1, LOW);
  digitalWrite(Player2, LOW);
  
  Serial.begin(9600);
}

// parsedInt from the device
int v;

void loop() {
 // Check for connection
 if (Serial.available() > 0) {
  // If it's established, get the int written
  v = Serial.parseInt();

  // Commandset
  // 0, ignored
  // 1 = Player 1 ON
  // 2 = Player 1 OFF
  // 3 = Player 2 ON
  // 4 = Player 2 OFF
  switch(v) {
    case 1:
      digitalWrite(Player1, HIGH);
      break;
    case 2:
      digitalWrite(Player1, LOW);
      break;
    case 3:
      digitalWrite(Player2, HIGH);
      break;
    case 4:
      digitalWrite(Player2, LOW);
      break;
    default:
      break;
  }
 } 
 else {
  attractLoop();
 }
}


// Cycle the lights every 30 seconds
void attractLoop() {
   
    // toggle between the two lights quicly
     for (int i = 0; i < 10; i++) {
      digitalWrite(Player1, HIGH);
      digitalWrite(Player2, LOW);
      delay(200);
      digitalWrite(Player1, LOW);
      digitalWrite(Player2, HIGH);
      delay(200);
    }

   for (int i = 0; i < 10; i++) {
      digitalWrite(Player1, HIGH);
      digitalWrite(Player2, LOW);
      delay(100);
      digitalWrite(Player1, LOW);
      digitalWrite(Player2, HIGH);
      delay(100);
    }

  // keep them on for 30 seconds
  digitalWrite(Player1, HIGH);
  digitalWrite(Player2, HIGH);
  delay(30000);

    
}