angular.module('contactApp', [])
.controller('contactController', ['$http', function($http) {
    var contactList = this;
    contactList.url = 'http://localhost:5000/api/Contact';
    contactList.contacts = [];

    contactList.AddContact = function() {
        var name = document.getElementById('name').value;
        var address = document.getElementById('address').value;
        var email = document.getElementById('email').value;
        var phone = document.getElementById('phone').value;
        var other = document.getElementById('other').value;
        var newContact = {'name': name, 
                          'address': address,
                          'email': email,
                          'phone': phone,
                          'other': other };
        $http.post(contactList.url, newContact)
                  .then(function (response) {
                      if (response.status === 201) {
                          $http.get(contactList.url)
                              .then(function(getResponse) {
                                  console.log(getResponse);
                                if (getResponse.status === 200) {
                                    //alert('Contact Saved')
                                    contactList.contacts = getResponse.data;
                                    document.getElementById('name').value = "";
                                    document.getElementById('address').value = "";
                                    document.getElementById('email').value = "";
                                    document.getElementById('phone').value = "";
                                    document.getElementById('other').value = "";
                                }
                          });
                      } else {
                          alert('Unable to save contact, please contact Matthew diabloazul14@gmail.com');
                          console.log(response);
                      }
                  });
    };

    contactList.GetAllContacts = function() {
        console.log('getAllContacts Called');
        $http.get(contactList.url)
        .then(function(getResponse) {
            console.log(getResponse);
          if (getResponse.status === 200) {
              contactList.contacts = getResponse.data;
          }
        });
    };

    contactList.GetAllContacts();
}]);