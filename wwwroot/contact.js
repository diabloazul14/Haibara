angular.module('contactApp', [])
    .controller('contactController', ['$http', function ($http) {
        var contactList = this;
        contactList.url = 'http://localhost:5000/api/Contact';
        contactList.contacts = [];
        contactList.searchedContacts = [];
        contactList.JWT = null;
        contactList.showMainPage = false;

        contactList.retrieveToken = function () {
            var requestBody = { "username": contactList.username, "password": contactList.password };
            var requestTokenUrl = "http://localhost:5000/api/Authentication";
            $http.post(requestTokenUrl, requestBody)
                .then(function (response) {
                    if (response.status === 200) {
                        contactList.JWT = response.data.token;
                        contactList.GetAllContacts();
                        contactList.showMainPage = true;
                    } else {
                        alert("Username or Password is Incorrect, please contact site admin.");
                    }
                });
        };


        contactList.AddContact = function () {
            var name = document.getElementById('name').value;
            var address = document.getElementById('address').value;
            var email = document.getElementById('email').value;
            var phone = document.getElementById('phone').value;
            var other = document.getElementById('other').value;
            var newContact = {
                'name': name,
                'address': address,
                'email': email,
                'phone': phone,
                'other': other
            };
            $http.defaults.headers.common.Authorization = "Bearer " + contactList.JWT;
            $http.post(contactList.url, newContact)
                .then(function (response) {
                    if (response.status === 201) {
                        $http.get(contactList.url)
                            .then(function (getResponse) {
                                console.log(getResponse);
                                if (getResponse.status === 200) {
                                    contactList.contacts = getResponse.data;
                                    contactList.searchedContacts.push(newContact);
                                    document.getElementById('name').value = "";
                                    document.getElementById('address').value = "";
                                    document.getElementById('email').value = "";
                                    document.getElementById('phone').value = "";
                                    document.getElementById('other').value = "";                                    
                                }
                            });
                    } else {
                        contactList.showMainPage = false;
                        alert('Unable to save contact, please contact Matthew diabloazul14@gmail.com');
                        console.log(response);
                    }
                });
        };

        contactList.GetAllContacts = function () {
            console.log('getAllContacts Called');
            $http.defaults.headers.common.Authorization = "Bearer " + contactList.JWT;
            $http.get(contactList.url)
                .then(function (getResponse) {
                    console.log(getResponse);
                    if (getResponse.status === 200) {
                        contactList.contacts = getResponse.data;
                        contactList.searchedContacts = contactList.contacts;
                    } else {
                        contactList.showMainPage = false;
                        alert("Couldn't retrieve Contacts, please contact Site Admin.");
                    }
                });
        };

        contactList.ClearAddContactFields = function () {
            document.getElementById('name').value = '';
            document.getElementById('address').value = '';
            document.getElementById('email').value = '';
            document.getElementById('phone').value = '';
            document.getElementById('other').value = '';
        };

        contactList.removeDuplicatesAtIndexes = function (list, listOfIndexes) {
            var modifiedList = list;
            while(list.length > 1) {
                modifiedList.splice(-1, 1);
            }
            return modifiedList;
        };

        contactList.removeDuplicates = function(list) {
            var tempList = list;
            list.forEach(function (element) {
                var indexesOfElementInList = [];
                var index = 0;
                list.forEach(function (innerElement) {                    
                    if (innerElement === element) {
                        indexesOfElementInList.push(index)
                    }
                    index += 1;
                });
                if (indexesOfElementInList.length > 1) {
                    tempList = contactList.removeDuplicatesAtIndexes(tempList, indexesOfElementInList);
                }
            });
            return tempList;
        };


        contactList.combine = function (list1, list2) {
            if (list1.length === 0 && list2.length ===0) {
                return [];
            } else if (list1.length === 0 && list2.length !== 0) {
                return list2;
            } else if (list1.length !== 1 && list2.length === 0) {
                return list1;
            } else {
                var tempList = list1.concat(list2);
                tempList = contactList.removeDuplicates(tempList);
                return tempList;    
            }
        }

        contactList.search = function () {
            var nameList = contactList.SearchName();
            var addressList = contactList.SearchAddress();
            var phoneList = contactList.SearchTelephone();
            var emailList = contactList.SearchEmail();
            var otherList = contactList.SearchOther();
            var nameAddresscombine = contactList.combine(nameList, addressList);
            var phoneEmailcombine = contactList.combine(phoneList, emailList);
            var nameAddressOthercombine = contactList.combine(nameAddresscombine, otherList);
            var combines = contactList.combine(nameAddressOthercombine, phoneEmailcombine);
            if (combines.length !== 0) {
                contactList.searchedContacts = combines;
            } else {
                contactList.searchedContacts = contactList.contacts;
            }
        };

        contactList.SearchName = function () {
            var tempListOfContacts = [];
            contactList.contacts.forEach(element => {
                if (element.name.includes(contactList.nameSearch)) {
                    tempListOfContacts.push(element);
                }
            });

            return tempListOfContacts;
        };

        contactList.SearchAddress = function () {
            var tempListOfContacts = [];
            contactList.contacts.forEach(element => {
                if (element.address.includes(contactList.addressSearch)) {
                    tempListOfContacts.push(element);
                }
            });

            return tempListOfContacts;
        };

        contactList.SearchEmail = function () {
            var tempListOfContacts = [];
            contactList.contacts.forEach(element => {
                if (element.email.includes(contactList.emailSearch)) {
                    tempListOfContacts.push(element);
                }
            });

            return tempListOfContacts;
        };

        contactList.SearchTelephone = function () {
            var tempListOfContacts = [];
            contactList.contacts.forEach(element => {
                if (element.phone.includes(contactList.phoneSearch)) {
                    tempListOfContacts.push(element);
                }
            });

            return tempListOfContacts;
        };

        contactList.SearchOther = function () {
            var tempListOfContacts = [];
            contactList.contacts.forEach(element => {
                if (element.other.includes(contactList.otherSearch)) {
                    tempListOfContacts.push(element);
                }
            });

            return tempListOfContacts;
        };


    }]);