function createEmployees(arr) {
    let collection = getContext().getCollection();
    let collectionLink = collection.getSelfLink();
    let curIndex = 0;
    let employeesDataArr = JSON.parse(arr);
    let totalItems = employeesDataArr.length;
    let docsCreatedData = [];
    let docsInErrorData = [];
    console.log(totalItems);

    //Check if employees array is empty
    if (!employeesDataArr) throw new Error("Empty poco");

    //No Items
    if (totalItems == 0) {
        getContext().getResponse().setBody("No Data provided, nothing created");
        return;
    } else {
        //Begin the call handle rest of call in callback method
        createEmployee(employeesDataArr[curIndex], checkAndCreateNext);
    }

    function createEmployee(employee, checkAndCreateNext) {
        //Add a new property
        employee.fullName = `${employee.firstName} ${employee.lastName}`;

        var options = { disableAutomaticIdGeneration: false, preTriggerInclude: ['setFullName'] };
        //Try to create the document
        var docCreated = collection.createDocument(
            collectionLink,
            employee,
            options,
            checkAndCreateNext
        );
    }

    function checkAndCreateNext(error, emp, options) {
        //If error update the docsCreated object
        if (error) {
            docsInErrorData.push({
                document: employeesDataArr[curIndex],
                status: "Error: " + JSON.stringify(error)
            });
        } else {
            docsCreatedData.push({
                document: employeesDataArr[curIndex],
                status: "Document Created: " + JSON.stringify(emp)
            });
        }

        curIndex++;
        if (curIndex < totalItems) {
            createEmployee(employeesDataArr[curIndex], checkAndCreateNext);
        } else {
            getContext()
                .getResponse()
                .setBody({
                    docsCreated: { count: docsCreatedData.length, docsCreatedData },
                    docsInError: { count: docsInErrorData.length, docsInErrorData }
                });
            return;
        }
    }
}
