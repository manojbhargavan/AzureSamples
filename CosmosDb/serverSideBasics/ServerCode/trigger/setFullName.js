function setFullName() {
    throw new Error("Ok this is ok");
    let context = getContext();
    let request = context.getRequest();

    let employeeObject = request.getBody();

    let fullName = "Jaffa"; //employeeObject.firstName + " " + employeeObject.lastName;
    employeeObject.fullName = fullName;

    request.setBody(employeeObject);
}