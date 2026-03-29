(function () {
  window.ClinicWidget = {
    init: function (config) {
      const container = document.createElement("div");
      container.id = "clinic-widget";
      container.style = `
        position: fixed;
        bottom: 20px;
        right: 20px;
        width: 300px;
        background: white;
        border: 1px solid #ccc;
        padding: 15px;
        z-index: 9999;
        font-family: Arial;
      `;

      container.innerHTML = `
        <h4>Book Appointment</h4>
        <input id="name" placeholder="Name" style="width:100%;margin-bottom:5px"/>
        <input id="phone" placeholder="Phone" style="width:100%;margin-bottom:5px"/>
        <select id="service" style="width:100%;margin-bottom:5px"></select>
        <input type="date" id="date" style="width:100%;margin-bottom:5px"/>
        <select id="slots" style="width:100%;margin-bottom:5px"></select>
        <button id="bookBtn" style="width:100%">Book</button>
      `;

      document.body.appendChild(container);

      // Load services
      fetch(`${config.apiBase}/public/services/${config.clinicId}`)
        .then(res => res.json())
        .then(data => {
          const serviceSelect = document.getElementById("service");
          data.forEach(s => {
            const opt = document.createElement("option");
            opt.value = s.id;
            opt.text = s.name;
            serviceSelect.appendChild(opt);
          });
        });

      // Load slots when date changes
      document.getElementById("date").addEventListener("change", function () {
        const date = this.value;

        fetch(`${config.apiBase}/public/slots?clinicId=${config.clinicId}&date=${date}`)
          .then(res => res.json())
          .then(slots => {
            const slotSelect = document.getElementById("slots");
            slotSelect.innerHTML = "";

            slots.forEach(s => {
              const opt = document.createElement("option");
              opt.value = s;
              opt.text = new Date(s).toLocaleTimeString();
              slotSelect.appendChild(opt);
            });
          });
      });

      // Booking
      document.getElementById("bookBtn").addEventListener("click", function () {
        const data = {
          clinicId: config.clinicId,
          serviceId: document.getElementById("service").value,
          patientName: document.getElementById("name").value,
          phone: document.getElementById("phone").value,
          appointmentDate: document.getElementById("slots").value
        };

        fetch(`${config.apiBase}/booking`, {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(data)
        })
          .then(res => res.json())
          .then(() => alert("Booking successful!"));
      });
    }
  };
})();