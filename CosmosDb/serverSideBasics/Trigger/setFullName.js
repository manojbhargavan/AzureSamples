function setFullName() {
    
    let context = getContext();
    let request = context.getRequest();

    let employeeObject = request.getBody();

    let fullName = "Jaffa"; //employeeObject.firstName + " " + employeeObject.lastName;
    employeeObject.fullNameTrigger = fullName;

    request.setBody(employeeObject);
}