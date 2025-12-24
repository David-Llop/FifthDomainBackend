# Fifth Domain Backend

This is the answer to the techinical challenge by David Llop Roig

## How to run

Open [API/API/API.slnx](API/API/API.slnx) solution in Visual Studio and everything is already set up. It's provided by an existing DB with some data examples, if the DB file was deleted, the code to recreate the data is already in the app, so everything should work fine. Provided DB comes with some extra data to properly display the sorting logic
Two dummy users are provided, `user1` and `user2`, with passwords `user1password` and `user2password`. They are provided with some dummy data to demo.

## Extras I didn't have time to include

Proper session managment using DB. A session table with entries created at login and a moving expiration date would do the trick. The first request made in the last 30 seconds before expiration, would update the expiration date to be current time + 5 minutes.
