# This is the Denizen script that generated 'minecraft.fds'

meta_extra_data_generator_task:
    type: task
    debug: false
    script:
    - definemap output:
        blocks: <server.material_types.filter[is_block].parse[name]>
        items: <server.material_types.filter[is_item].parse[name]>
        particles: <server.particle_types>
        effects: <server.effect_types>
        sounds: <server.sound_types>
        entities: <server.entity_types>
        enchantments: <server.enchantment_keys>
        biomes: <server.biome_types.parse[name]>
        attributes: <server.nbt_attribute_types>
        gamerules: <server.gamerules>
        potion_effects: <server.potion_effect_types>
        potions: <server.potion_types>
        statistics: <server.statistic_types>
    - log type:none <[output].to_yaml> file:extra_data.fds
