CREATE TABLE foods(
    id SERIAL,
    name VARCHAR,
    image_url VARCHAR,
    kcal FLOAT,
    protein FLOAT,
    fat FLOAT,
    carbs FLOAT,
    water FLOAT,
    cellulose FLOAT,
    organic_acids FLOAT,
    glycemic_index FLOAT,
    cholesterol FLOAT,
    saturated_fats FLOAT,
    vit_a FLOAT,
    vit_b1 FLOAT,
    bit_b2 FLOAT,
    vit_b3_pp FLOAT,
    vit_e FLOAT,
    vit_c FLOAT,
    vit_b4 FLOAT,
    vit_b5 FLOAT,
    vit_b6 FLOAT,
    vit_b9 FLOAT,
    vit_k FLOAT,
    vit_h FLOAT,
    vit_d FLOAT,
    potassium FLOAT,
    calcium FLOAT,
    magnesium FLOAT,
    phosphorus FLOAT,
    sodium FLOAT,
    zinc FLOAT,
    iron FLOAT,
    selenium FLOAT,
    copper FLOAT,
    manganese FLOAT,
    fluorine FLOAT,
    iodine FLOAT,
    sulfur FLOAT,
    chromium FLOAT,
    silicon FLOAT,
    CONSTRAINT products_pk PRIMARY KEY (id),
    CONSTRAINT name_not_null CHECK ( name IS NOT NULL )
);

CREATE TABLE products(
    id INT,
    is_foundational BOOLEAN,
    CONSTRAINT products_pk PRIMARY KEY (id),
    CONSTRAINT products_food_fk FOREIGN KEY (id) REFERENCES foods(id) ON DELETE CASCADE ,
    CONSTRAINT food_id_not_null CHECK ( id IS NOT NULL ),
    CONSTRAINT is_foundational_specified CHECK ( is_foundational IS NOT NULL )
);

CREATE TABLE dishes(
    id INT,
    recipe TEXT,
    CONSTRAINT dishes_pk PRIMARY KEY (id),
    CONSTRAINT food_id_fk FOREIGN KEY (id) REFERENCES foods(id) ON DELETE CASCADE ,
    CONSTRAINT food_id_specified CHECK ( id IS NOT NULL ),
    CONSTRAINT recipe_not_null CHECK ( recipe IS NOT NULL )
);

CREATE TABLE cooking_appliances(
    id SERIAL,
    name VARCHAR,
    image_url VARCHAR,
    CONSTRAINT cooking_appliances_pk PRIMARY KEY(id),
    CONSTRAINT name_not_null CHECK ( name IS NOT NULL )
);

CREATE TABLE dish_products(
    product_id INT,
    dish_id INT,
    CONSTRAINT dish_products_pk PRIMARY KEY(product_id, dish_id),
    CONSTRAINT product_id_fk FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE,
    CONSTRAINT dish_id_fk FOREIGN KEY (dish_id) REFERENCES dishes(id) ON DELETE CASCADE,
    CONSTRAINT product_id_not_null CHECK ( product_id IS NOT NULL ),
    CONSTRAINT dish_id_not_null CHECK (dish_id IS NOT NULL)
);

CREATE TABLE dish_appliance(
    dish_id INT,
    appliance_id INT,
    CONSTRAINT dish_appliance_pk PRIMARY KEY (dish_id, appliance_id),
    CONSTRAINT appliance_id_fk FOREIGN KEY (appliance_id) REFERENCES cooking_appliances(id) ON DELETE CASCADE,
    CONSTRAINT dish_id_fk FOREIGN KEY (dish_id) REFERENCES dishes(id) ON DELETE CASCADE,
    CONSTRAINT dish_id_not_null CHECK ( dish_id IS NOT NULL ),
    CONSTRAINT appliance_id_not_null CHECK ( appliance_id IS NOT NULL )
);




