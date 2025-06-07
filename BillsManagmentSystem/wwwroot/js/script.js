let invoices = [];

function addInvoice() {
    const invoiceNumberInput = document.getElementById("invoicNo");
    const VNDCodeInput = document.getElementById("VNDCode");
    const BILDATInput = document.getElementById("BILDAT");
    const imageInput = document.getElementById("Image");

    const invoiceNumber = invoiceNumberInput.value;
    const VNDCode = VNDCodeInput.value;
    const BILDAT = BILDATInput.value;
    const image = imageInput.value;

    if (invoiceNumber && VNDCode && BILDAT && image) {
        const newInvoice = { invoiceNumber, VNDCode, BILDAT, image };
        invoices.push(newInvoice);
        updateTable();

        // Clear the form after adding an invoice
        invoiceNumberInput.value = "";
        VNDCodeInput.value = "";
        BILDATInput.value = "";
        imageInput.value = "";

        // Remove the form
        removeForm();
    } else {
        alert("Please provide valid input for all fields.");
    }
}

function updateTable() {
    const tableContainer = document.getElementById("invoices-table");

    // If the table is empty and there are no invoices, do not display it
    if (invoices.length === 0 && tableContainer.children.length === 0) {
        return;
    }

    // Clear existing content
    tableContainer.innerHTML = "";

    const table = document.createElement("table");
    const headerRow = table.insertRow(0);

    const columns = ["Invoice Number", "VNDCode", "BILDAT", "Image", "Actions"];
    for (const col of columns) {
        const th = document.createElement("th");
        th.textContent = col;
        headerRow.appendChild(th);
    }

    for (const invoice of invoices) {
        const row = table.insertRow(-1);

        const cell1 = row.insertCell(0);
        cell1.textContent = invoice.invoiceNumber;

        const cell2 = row.insertCell(1);
        cell2.textContent = invoice.VNDCode;

        const cell3 = row.insertCell(2);
        cell3.textContent = invoice.BILDAT;

        const cell4 = row.insertCell(3);
        cell4.textContent = invoice.image;

        const cell5 = row.insertCell(4);
        const updateButton = document.createElement("button");
        updateButton.textContent = "Update";
        updateButton.addEventListener("click", () => updateInvoice(invoice));
        cell5.appendChild(updateButton);

        const deleteButton = document.createElement("button");
        deleteButton.textContent = "Delete";
        deleteButton.addEventListener("click", () => deleteInvoice(invoice));
        cell5.appendChild(deleteButton);
    }

    // Append the new table to the existing content
    tableContainer.appendChild(table);
}

function updateInvoice(invoice) {
    // Placeholder for update functionality
    console.log(`Update invoice ${invoice.invoiceNumber}`);
}

function deleteInvoice(invoice) {
    // Placeholder for delete functionality
    console.log(`Delete invoice ${invoice.invoiceNumber}`);
}

function removeForm() {
    const form = document.getElementById("invoice-form");
    form.parentNode.removeChild(form);
}

// Initial table load
updateTable();
